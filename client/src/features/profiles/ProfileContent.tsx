import { Tab } from "semantic-ui-react";
import { Profile } from "../../app/models/profile";
import { observer } from "mobx-react-lite";
import ProfilePhotos from "./ProfilePhotos";
import ProfileAbout from "./ProfileAbout";

interface Props {
  profile: Profile;
}

const ProfileContent = ({ profile }: Props) => {
  const panes = [
    { menuItem: "About", render: () => <ProfileAbout profile={profile} /> },
    { menuItem: "Photos", render: () => <ProfilePhotos profile={profile} /> },
    { menuItem: "Events", render: () => <Tab.Pane content="Events Content" /> },
    {
      menuItem: "Followers",
      render: () => <Tab.Pane content="Followers Content" />,
    },
    {
      menuItem: "Following",
      render: () => <Tab.Pane content="Following Content" />,
    },
  ];

  return (
    <Tab
      menu={{ vertical: true, fluid: true }}
      panes={panes}
      menuPosition="right"
    />
  );
};

export default observer(ProfileContent);
