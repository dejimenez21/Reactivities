import { observer } from "mobx-react-lite";
import React from "react";
import { Link } from "react-router-dom";
import { Button, Header, Item, Segment, Image, Label } from "semantic-ui-react";
import { Activity } from "../../../app/models/activity";
import { format } from "date-fns";
import { useStore } from "../../../app/stores/store";

const activityImageStyle = {
  filter: "brightness(30%)",
};

const activityImageTextStyle = {
  position: "absolute",
  bottom: "5%",
  left: "5%",
  width: "100%",
  height: "auto",
  color: "white",
};

interface Props {
  activity: Activity | undefined;
}

export default observer(function ActivityDetailedHeader({ activity }: Props) {
  const {
    activityStore: { updateAttendance, cancelActivityToggle, loading },
  } = useStore();
  return (
    <Segment.Group>
      <Segment basic attached="top" style={{ padding: "0" }}>
        {activity?.isCanceled && (
          <Label
            ribbon
            color="red"
            style={{ position: "absolute", zIndex: 1000, top: 20, left: -14 }}
          >
            Canceled
          </Label>
        )}
        <Image
          src={`/assets/categoryImages/${activity?.category}.jpg`}
          fluid
          style={activityImageStyle}
        />
        <Segment style={activityImageTextStyle} basic>
          <Item.Group>
            <Item>
              <Item.Content>
                <Header
                  size="huge"
                  content={activity?.title}
                  style={{ color: "white" }}
                />
                <p>{format(activity?.date!, "dd MMM yyyy")}</p>
                <p>
                  Hosted by{" "}
                  <strong>
                    <Link to={`/profiles/${activity?.host?.username}`}>
                      {activity?.host?.displayName}
                    </Link>
                  </strong>
                </p>
              </Item.Content>
            </Item>
          </Item.Group>
        </Segment>
      </Segment>
      <Segment clearing attached="bottom">
        {activity?.isHost ? (
          <>
            <Button
              as={Link}
              disabled={activity.isCanceled}
              to={`/manage/${activity?.id}`}
              color="orange"
              floated="right"
            >
              Manage Event
            </Button>
            <Button
              color={activity.isCanceled ? "green" : "red"}
              floated="left"
              basic
              onClick={cancelActivityToggle}
              loading={loading}
              content={
                activity.isCanceled ? "Re-activate Activity" : "Cancel Activity"
              }
            />
          </>
        ) : activity?.isGoing ? (
          <Button onClick={updateAttendance} loading={loading}>
            Cancel attendance
          </Button>
        ) : (
          <Button
            onClick={updateAttendance}
            loading={loading}
            color="teal"
            disabled={activity?.isCanceled}
          >
            Join Activity
          </Button>
        )}
      </Segment>
    </Segment.Group>
  );
});
