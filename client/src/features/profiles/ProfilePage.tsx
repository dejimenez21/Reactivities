import { useParams } from "react-router-dom";
import { Grid } from "semantic-ui-react";
import ProfileHeader from "./ProfileHeader";
import ProfileContent from "./ProfileContent";
import { useStore } from "../../app/stores/store";
import { useEffect } from "react";
import LoadingComponent from "../../app/layout/LoadingComponent";
import { observer } from "mobx-react-lite";

const ProfilePage = () => {
  const { username } = useParams();
  const { profileStore } = useStore();
  const { loadProfile, loadingProfile, profile } = profileStore;

  useEffect(() => {
    if (username) loadProfile(username);
  }, [loadProfile, username]);

  if (loadingProfile) return <LoadingComponent content="Loading profile..." />;

  return (
    <Grid>
      <Grid.Column width={16}>
        <ProfileHeader profile={profile!} />
        <ProfileContent profile={profile!} />
      </Grid.Column>
    </Grid>
  );
};

export default observer(ProfilePage);
