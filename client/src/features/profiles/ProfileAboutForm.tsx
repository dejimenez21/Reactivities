import { Form, Formik } from "formik";
import { observer } from "mobx-react-lite";
import { Button } from "semantic-ui-react";
import MyTextInput from "../../app/common/form/MyTextInput";
import MyTextArea from "../../app/common/form/MyTextArea";
import { AboutFormValues } from "../../app/models/profile";

interface Props {
  about: AboutFormValues;
  onFormSubmit: (values: AboutFormValues) => void;
}

const ProfileAboutForm = ({ about, onFormSubmit }: Props) => {
  return (
    <Formik onSubmit={onFormSubmit} initialValues={{ ...about }}>
      {({ handleSubmit, isSubmitting, dirty }) => (
        <Form autoComplete="off" onSubmit={handleSubmit} className="ui form">
          <MyTextInput placeholder="Display Name" name="displayName" />
          <MyTextArea placeholder="Add your bio" name="bio" rows={8} />
          <Button
            floated="right"
            color="green"
            content="Update profile"
            type="submit"
            loading={isSubmitting}
            disabled={!dirty}
          />
        </Form>
      )}
    </Formik>
  );
};

export default observer(ProfileAboutForm);
