import { useParams } from "react-router-dom";
import { Grid, Item, Segment } from "semantic-ui-react";
import ProfileHeader from "./ProfileHeader";
import ProfileContent from "./ProfileContent";

const ProfilePage = () => {
  const { username } = useParams();

  return (
    <Grid>
      <Grid.Column width={16}>
        <ProfileHeader />
        <ProfileContent />
      </Grid.Column>
    </Grid>
  );
};

export default ProfilePage;
