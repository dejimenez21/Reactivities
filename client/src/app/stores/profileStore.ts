import { makeAutoObservable, runInAction } from "mobx";
import { Photo, Profile } from "../models/profile";
import agent from "../api/agent";
import { store } from "./store";

export default class ProfileStore {
  profile: Profile | null = null;
  loadingProfile = false;
  uploadingPhoto = false;
  loading = false;

  constructor() {
    makeAutoObservable(this);
  }

  get isCurrentUser() {
    if (this.profile && store.userStore.user) {
      return this.profile.username === store.userStore.user.username;
    }
    return false;
  }

  loadProfile = async (username: string) => {
    this.loadingProfile = true;
    try {
      const profile = await agent.Profiles.get(username);
      runInAction(() => {
        this.profile = profile;
      })
    } catch (error) {       
      console.log(error);
    } finally {
      runInAction(() => {
        this.loadingProfile = false;
      })
    }
  };

  uploadPhoto = async (file: Blob) => {
    this.uploadingPhoto = true;
    try {
      const response = await agent.Profiles.uploadPhoto(file);
      const photo = response.data;
      runInAction(() => {
        if (this.profile) {
          this.profile.photos?.push(photo);
          if (photo.isMain) {
            store.userStore.setImage(photo.url);
            this.profile.image = photo.url;
          }
        }
      })
    } catch (error) {
      console.log(error);
    } finally {
      runInAction(() => {
        this.uploadingPhoto = false;
      })
    }
  }

  setMainPhoto = async (photo: Photo) => {
    this.loading = true;
    try {
      await agent.Profiles.setMainPhoto(photo.id);
      store.userStore.setImage(photo.url);
      runInAction(() => {
        if (this.profile && this.profile.photos) {
          this.profile.photos?.forEach(p => p.isMain = false);
          this.profile.photos!.find(p => p.id === photo.id)!.isMain = true;
          this.profile.image = photo.url;
        }
      })
    } catch (error) {
      console.log(error);
    } finally {
      runInAction(() => {
        this.loading = false;
      })
    }
  }
  
  deletePhoto = async (id: string) => {
    this.loading = true;
    try {
      await agent.Profiles.deletePhoto(id);
      runInAction(() => {
        if (this.profile && this.profile.photos) {
          this.profile.photos = this.profile.photos.filter(p => p.id !== id);
        }
      })
    } catch (error) {
      console.log(error);
    } finally {
      runInAction(() => {
        this.loading = false;
      })
    }
  }
}
