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

  const handleCancelEdit = () => {
    setEditMode(false);
  };

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
          {isCurrentUser &&
            (editMode ? (
              <Button
                content="Cancel"
                floated="right"
                onClick={handleCancelEdit}
              />
            ) : (
              <Button
                content="Edit Profile"
                floated="right"
                onClick={() => setEditMode(true)}
              />
            ))}
        </Grid.Column>
        <Grid.Column width={16}>
          {editMode ? (
            <ProfileAboutForm
              about={{ displayName: profile.displayName, bio: profile.bio }}
              onFormSubmit={handleFormSubmit}
            />
          ) : (
            <>{profile?.bio}</>
          )}
        </Grid.Column>
      </Grid>
    </Tab.Pane>
  );
};

export default observer(ProfileAbout);
