import { Tab } from "semantic-ui-react";

const ProfileContent = () => {

    const panes = [
        { menuItem: 'About', render: () => <Tab.Pane content='About Content' /> },
        { menuItem: 'Photos', render: () => <Tab.Pane content='Photos Content' /> },
        { menuItem: 'Events', render: () => <Tab.Pane content='Events Content' /> },
        { menuItem: 'Followers', render: () => <Tab.Pane content='Followers Content' /> },
        { menuItem: 'Following', render: () => <Tab.Pane content='Following Content' /> }
    ];
    
    return (
        <Tab menu={{ vertical: true, fluid: true }} panes={panes} menuPosition="right" />
    );
}

export default ProfileContent;