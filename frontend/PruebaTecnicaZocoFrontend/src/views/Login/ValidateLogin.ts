const validateLogin = (values) => {
    const errors = {};

    if (!values.email) {
        errors.email = "El nombre de usuario es obligatorio";
    } else if (values.email.length < 3) {
        errors.email = "El nombre de usuario debe tener al menos 3 caracteres";
    }

    if (!values.password) {
        errors.password = "La contraseña es obligatoria";
    }

    return errors;
};

export default validateLogin;