import { observer } from "mobx-react-lite";
import { Card, Icon, Image } from "semantic-ui-react";
import { Profile } from "../../app/models/profile";
import { Link } from "react-router-dom";

interface Props {
    profile: Profile;
}

function ProfileCard({profile}: Props) {

    const truncateBio = (bio?: string) => {
        if(bio && bio.length > 40) {
            const cropped = bio.substring(0, 40);
            return cropped + '...';
        }
        return bio;
    }

    return (
        <Card as={Link} to={`/profiles/${profile.username}`}>
            <Image src={profile.image || '/assets/user.png'} />
            <Card.Content>
                <Card.Header content={profile.displayName} />
                <Card.Description content={truncateBio(profile.bio)} />
            </Card.Content>
            <Card.Content extra>
                <Icon name="user" />
                20 followers
            </Card.Content>
        </Card>
    )
}

export default observer(ProfileCard);