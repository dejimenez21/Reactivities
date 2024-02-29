import { Link, NavLink } from "react-router-dom";
import { Button, Container, Dropdown, DropdownMenu, Menu, Image, DropdownItem } from "semantic-ui-react";
import { useStore } from "../stores/store";
import { observer } from "mobx-react-lite";

function NavBar() {
  const {userStore: {user, logout}} = useStore();
  return (
    <Menu inverted fixed="top">
      <Container>
        <Menu.Item as={NavLink} to="/" header>
          <img
            src="/assets/logo.png"
            alt="logo"
            style={{ marginRight: "10px" }}
          />
          Reactivities
        </Menu.Item>
        <Menu.Item as={NavLink} to="/activities" name="Activities" />
        <Menu.Item as={NavLink} to="/errors" name="Errors" />
        <Menu.Item>
          <Button
            positive
            content="Create Activity"
            as={NavLink}
            to="/createActivity"
          />
        </Menu.Item>
        <Menu.Item position="right">
          <Image src={user?.image || 'assets/user.png'} avatar spaced="right" />
          <Dropdown pointing="top left" text={user?.displayName}>
            <DropdownMenu>
              <DropdownItem as={Link} to={`profile/${user?.username}`} content="My Profile" icon='user' />
              <DropdownItem onClick={logout} content="Logout" icon="power" />
            </DropdownMenu>
          </Dropdown>
        </Menu.Item>
      </Container>
    </Menu>
  );
}

export default observer(NavBar);
