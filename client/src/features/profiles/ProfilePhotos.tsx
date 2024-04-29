import { observer } from "mobx-react-lite";
import { Card, Image, Tab, Header, Grid, Button } from "semantic-ui-react";
import { Photo, Profile } from "../../app/models/profile";
import { useStore } from "../../app/stores/store";
import { SyntheticEvent, useState } from "react";
import PhotoUploadWidget from "../../app/common/ImageUpload/PhotoUploadWidget";

interface Props {
  profile: Profile;
}

const ProfilePhotos = ({ profile }: Props) => {
  const { profileStore } = useStore();
  const { isCurrentUser, uploadPhoto, uploadingPhoto, loading, setMainPhoto, deletePhoto } = profileStore;
  const [addPhotoMode, setAddPhotoMode] = useState(false);
  const [target, setTarget] = useState('');

  const handleSetMainPhoto = (photo: Photo, event: SyntheticEvent<HTMLButtonElement>) => {
    setTarget(event.currentTarget.name);
    setMainPhoto(photo);
  }

  const handleDeletePhoto = (photo: Photo, event: SyntheticEvent<HTMLButtonElement>) => {
    setTarget(event.currentTarget.name);
    deletePhoto(photo.id);
  }

  const handlePhotoUpload = (photo: Blob) => {
    uploadPhoto(photo).then(() => setAddPhotoMode(false));
  };

  return (
    <Tab.Pane>
      <Grid>
        <Grid.Column width={16}>
          <Header floated="left" icon="image" content="Photos" />
          {isCurrentUser && (
            <Button
              floated="right"
              content={addPhotoMode ? "Cancel" : "Add photo"}
              onClick={() => setAddPhotoMode(!addPhotoMode)}
            />
          )}
        </Grid.Column>
        <Grid.Column width={16}>
          {addPhotoMode ? (
            <PhotoUploadWidget
              uploadPhoto={handlePhotoUpload}
              loading={uploadingPhoto}
            />
          ) : (
            <Card.Group itemsPerRow={5}>
              {profile.photos?.map((photo) => (
                <Card key={photo.id}>
                  <Image src={photo.url} />
                  {isCurrentUser && (
                    <Button.Group widths={2} fluid>
                      <Button
                        basic
                        color="green"
                        content="Main"
                        name={photo.id}
                        loading={loading && target === photo.id}
                        disabled={photo.isMain}
                        onClick={(e) => handleSetMainPhoto(photo, e)}
                      />
                      <Button
                        basic
                        color="red"
                        icon='trash'
                        name={photo.id + 'd'}
                        disabled={photo.isMain}
                        loading={loading && target === photo.id + 'd'}
                        onClick={(e) => handleDeletePhoto(photo, e)}
                      />
                    </Button.Group>
                  )}
                </Card>
              ))}
            </Card.Group>
          )}
        </Grid.Column>
      </Grid>
    </Tab.Pane>
  );
};

export default observer(ProfilePhotos);
