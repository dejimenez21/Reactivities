import { makeAutoObservable, runInAction } from "mobx";
import agent from "../api/agent";
import { Activity, ActivityFormValues } from "../models/activity";
import { v4 as uuid } from "uuid";
import { format } from "date-fns";
import { store } from "./store";

export default class ActivityStore {
  activityRegistry = new Map<string, Activity>();
  selectedActivity: Activity | undefined = undefined;
  loading = false;
  loadingInitial = false;

  constructor() {
    makeAutoObservable(this);
  }

  get activitiesByDate() {
    return Array.from(this.activityRegistry.values()).sort(
      (a, b) => a.date!.getTime() - b.date!.getTime()
    );
  }

  get groupedActivities() {
    return Object.entries(
      this.activitiesByDate.reduce((activities, activity) => {
        const date = format(activity.date!, "dd MMM yyyy");
        activities[date] = activities[date]
          ? [...activities[date], activity]
          : [activity];
        return activities;
      }, {} as { [key: string]: Activity[] })
    );
  }

  loadActivities = async () => {
    this.setLoadingInitial(true);
    try {
      const activities = await agent.Activities.list();
      runInAction(() =>
        activities.forEach((activity) => {
          this.setActivity(activity);
        })
      );
      this.setLoadingInitial(false);
    } catch (error) {
      console.log(error);
      this.setLoadingInitial(false);
    }
  };

  loadActivity = async (id: string) => {
    let activity = this.getActivity(id);
    if (activity) {
      this.setSelectedActivity(activity);
      return activity;
    } else {
      this.setLoadingInitial(true);
      try {
        activity = await agent.Activities.details(id);
        this.setActivity(activity);
        this.setLoadingInitial(false);
        this.setSelectedActivity(activity);
        return activity;
      } catch (error) {
        console.log(error);
        this.setLoadingInitial(false);
      }
    }
  };

  private refreshActivity = async (id: string) => {
    try {
      let activity = await agent.Activities.details(id);
      this.setActivity(activity);
      this.setSelectedActivity(activity);
      return activity;
    } catch (error) {
      console.log(error);
    }
  };

  private setActivity = (activity: Activity) => {
    const user = store.userStore.user;
    if (user) {
      activity.isGoing = activity.attendees?.some(
        (a) => a.username === user.username
      );
      activity.isHost = activity.hostUsername === user.username;
      activity.host = activity.attendees?.find(
        (a) => a.username === activity.hostUsername
      );
    }
    activity.date = new Date(activity.date!);
    this.activityRegistry.set(activity.id, activity);
  };

  private getActivity = (id: string) => this.activityRegistry.get(id);

  setLoadingInitial = (state: boolean) => {
    this.loadingInitial = state;
  };

  setSelectedActivity = (activity: Activity) => {
    this.selectedActivity = activity;
  };

  createActivity = async (activityValues: ActivityFormValues) => {
    activityValues.id = uuid();

    try {
      const activity = await agent.Activities.create(activityValues);
      this.setActivity(activity);
      this.setSelectedActivity(activity);
    } catch (error) {
      console.log(error);
    } 

    return activityValues.id;
  };

  updateActivity = async (activityValues: ActivityFormValues) => {
    try {
      const {id: activityId} = await agent.Activities.update(activityValues);
      const updatedActivity = {...this.getActivity(activityId), ...activityValues} as Activity;
      this.setActivity(updatedActivity);
      this.setSelectedActivity(updatedActivity);
    } catch (error) {
      console.log(error);
    }
  };

  deleteActivity = async (id: string) => {
    this.loading = true;
    try {
      await agent.Activities.delete(id);
      runInAction(() => {
        this.activityRegistry.delete(id);
        this.loading = false;
      });
    } catch (error) {
      console.log(error);
      this.loading = false;
    }
  };

  updateAttendance = async () => {
    this.loading = true;
    try {
      await agent.Activities.attend(this.selectedActivity!.id);
      await this.refreshActivity(this.selectedActivity!.id);
    } catch (error) {
      console.log(error);
    } finally {
      runInAction(() => (this.loading = false));
    }
  };

  cancelActivityToggle = async () => {
    await this.updateAttendance();
  }

  clearSelectedActivity = () => {
    this.selectedActivity = undefined;
  }
}
