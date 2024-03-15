import { observer } from "mobx-react-lite";
import { Image, List, Popup } from "semantic-ui-react";
import { Profile } from "../../../app/models/profile";
import { Link } from "react-router-dom";
import ProfileCard from "../../profiles/ProfileCard";

interface Props {
  attendees: Profile[];
}

function ActivityListItemAttendee({ attendees }: Props) {
  return (
    <List horizontal>
      {attendees.map((attendee) => (
        <List.Item
          key={attendee.username}
          as={Link}
          to={`profiles/${attendee.username}`}
        >
          <Popup
            hoverable
            content={<ProfileCard profile={attendee} />}
            trigger={
              <Image
                size="mini"
                circular
                src={attendee.image || "/assets/user.png"}
              />
            }
          />
        </List.Item>
      ))}
    </List>
  );
}

export default observer(ActivityListItemAttendee);
