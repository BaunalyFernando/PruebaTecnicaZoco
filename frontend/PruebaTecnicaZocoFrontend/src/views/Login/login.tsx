import { Formik, Form, Field, ErrorMessage } from "formik";
import validateLogin from "./ValidateLogin";
import styles from "./Login.module.css";
import { Link, useNavigate } from "react-router-dom";
import Swal from "sweetalert2";
import { useContext } from "react";
import axios from "axios";

const Login = () => {
    const navigate = useNavigate();

    const initialValues = {
        email: "",
        password: "",
    };
    
    const handleSubmit = async (values) => {
        console.log("Submit", values);
        
        try {
            const response = await axios.post("http://localhost:5186/auth/login", values);
            console.log("Respuesta completa:", response);
            
            if (response && response.data && response.data.token) {
                localStorage.setItem("token", response.data.token);
                localStorage.setItem("role", response.data.role);
                localStorage.setItem("userId", response.data.id);
                
                Swal.fire({
                    icon: "success",
                    title: "Usuario logueado correctamente.",
                });
                navigate("/home");
            } else {
                Swal.fire({
                    icon: "error",
                    title: "Error de respuesta",
                    text: "Formato de respuesta inesperado.",
                });
            }
        } catch (error) {
            console.error("Error:", error);
        
            const errorMessage =
                error.response?.data?.message || 
                error.response?.data || 
                "Credenciales incorrectas o servidor no disponible.";
        
                Swal.fire({
                    icon: "error",
                    title: "Error al iniciar sesión",
                    text:
                      error.response?.data?.error ||           
                      error.response?.data?.message || 
                      error.response?.data?.title || 
                      "Credenciales incorrectas o servidor no disponible.",
                  });
        }
    };
    
    return (
        <div className={styles.container}>
            <h1 className={styles.title}>Iniciar Sesión</h1>
            <div className={styles.card}>
                <Formik
                    initialValues={initialValues}
                    validate={validateLogin}
                    onSubmit={handleSubmit}
                >
                    <Form className={styles.form}>
                        <div className={styles.formGroup}>
                            <label htmlFor="email">Email</label>
                            <Field
                                name="email"
                                type="text"
                                className={styles.input}
                            />
                            <ErrorMessage
                                name="email"
                                component="div"
                                className={styles.error}
                            />
                        </div>
                        <div className={styles.formGroup}>
                            <label htmlFor="password">Contraseña</label>
                            <Field
                                name="password"
                                type="password"
                                className={styles.input}
                            />
                            <ErrorMessage
                                name="password"
                                component="div"
                                className={styles.error}
                            />
                        </div>
                        <button type="submit" className={styles.button}>
                            Log In
                        </button>
                        <br />
                        <label>
                           Todavia no tenes cuenta? <Link to="/register" className={styles.link}>Registrate aqui</Link>
                        </label>
                    </Form>
                </Formik>
            </div>
        </div>
    );
};

export default Login;