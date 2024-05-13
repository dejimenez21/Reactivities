import { observer } from "mobx-react-lite";
import { useState } from "react";
import { Button, Grid, Header, Tab } from "semantic-ui-react";
import { AboutFormValues, Profile } from "../../app/models/profile";
import ProfileAboutForm from "./ProfileAboutForm";
import { useStore } from "../../app/stores/store";

interface Props {
  profile: Profile;
}

const ProfileAbout = ({ profile }: Props) => {
  const [editMode, setEditMode] = useState(false);
  const { profileStore } = useStore();
  const { isCurrentUser, updateAbout } = profileStore;

  const handleFormSubmit = async (values: AboutFormValues) => {
    await updateAbout(values);
    setEditMode(false);
  };

  return (
    <Tab.Pane>
      <Grid>
        <Grid.Column width={16}>
          <Header
            floated="left"
            icon="user"
            content={`About ${profile?.displayName}`}
          />
          {isCurrentUser && (
            <Button
              content={editMode ? "Cancel" : "Edit Profile"}
              floated="right"
              onClick={() => setEditMode(!editMode)}
              basic
            />
          )}
        </Grid.Column>
        <Grid.Column width={16}>
          {editMode ? (
            <ProfileAboutForm
              about={{ displayName: profile.displayName, bio: profile.bio }}
              onFormSubmit={handleFormSubmit}
            />
          ) : (
            <span style={{ whiteSpace: "pre-wrap" }}>{profile?.bio}</span>
          )}
        </Grid.Column>
      </Grid>
    </Tab.Pane>
  );
};

export default observer(ProfileAbout);
