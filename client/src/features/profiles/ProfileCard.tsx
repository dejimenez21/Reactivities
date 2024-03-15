import { observer } from "mobx-react-lite";
import { Card, Icon, Image } from "semantic-ui-react";
import { Profile } from "../../app/models/profile";
import { Link } from "react-router-dom";

interface Props {
    profile: Profile;
}

function ProfileCard({profile}: Props) {
    return (
        <Card as={Link} to={`/profiles/${profile.username}`}>
            <Image src={profile.image || '/assets/user.png'} />
            <Card.Content>
                <Card.Header content={profile.displayName} />
                <Card.Description content='Bio goes here' />
            </Card.Content>
            <Card.Content extra>
                <Icon name="user" />
                20 followers
            </Card.Content>
        </Card>
    )
}

export default observer(ProfileCard);