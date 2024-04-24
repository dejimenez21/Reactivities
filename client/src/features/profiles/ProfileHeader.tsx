import {
  Button,
  Divider,
  Grid,
  Header,
  Item,
  Reveal,
  Segment,
  Statistic,
} from "semantic-ui-react";

const ProfileHeader = () => {
  return (
    <Segment>
      <Grid>
        <Grid.Column width={12}>
          <Item.Group>
            <Item>
              <Item.Image src={"/assets/user.png"} avatar size="small" />
              <Item.Content verticalAlign="middle">
                <Header as="h1">Displayname</Header>
              </Item.Content>
            </Item>
          </Item.Group>
        </Grid.Column>
        <Grid.Column width={4} textAlign="center">
          <Statistic.Group widths={2}>
            <Statistic label="Followers" value={5} />
            <Statistic label="Following" value={42} />
          </Statistic.Group>
          <Divider />
          <Reveal animated="move">
            <Reveal.Content visible style={{ width: "100%" }}>
              <Button color="teal" fluid content="Following" />
            </Reveal.Content>
            <Reveal.Content hidden style={{ width: "100%" }}>
              <Button
                color={true ? "red" : "green"}
                fluid
                content={true ? "Unfollow" : "Follow"}
                basic
              />
            </Reveal.Content>
          </Reveal>
        </Grid.Column>
      </Grid>
    </Segment>
  );
};

export default ProfileHeader;
