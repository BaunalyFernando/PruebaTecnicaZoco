import React, { useState } from 'react';
import axios from 'axios';
import styles from '../home/Home.module.css';
import Navbar from '../../components/Navbar';
import { useNavigate } from 'react-router-dom';

const Register = () => {
  const navigate = useNavigate();
  const [form, setForm] = useState({
    nombre: '',
    apellido: '',
    email: '',
    password: '',
  });

  const [error, setError] = useState('');
  const [success, setSuccess] = useState('');

  const handleChange = (e) => {
    setForm({
      ...form,
      [e.target.name]: e.target.value
    });
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    setError('');
    setSuccess('');

    axios.post(`${import.meta.env.VITE_API_BASE_URL}/api/Users/CreateUser`, form)
      .then(() => {
        setSuccess('Usuario registrado correctamente.');
        setTimeout(() => navigate('/login'), 2000);
      })
      .catch((err) => {
        const msg = err.response?.data || 'Error al registrar usuario.';
        setError(typeof msg === 'string' ? msg : 'Error inesperado.');
      });
  };

  return (
    <div className={styles.container}>
      <Navbar />

      <div className={styles.content}>
        <div className={styles.section}>
          <h2>Registro de Usuario</h2>
          <form onSubmit={handleSubmit} className={styles.form}>
            <input
              type="text"
              name="nombre"
              placeholder="Nombre"
              value={form.nombre}
              onChange={handleChange}
              required
            />
            <input
              type="text"
              name="apellido"
              placeholder="Apellido"
              value={form.apellido}
              onChange={handleChange}
              required
            />
            <input
              type="email"
              name="email"
              placeholder="Correo electrónico"
              value={form.email}
              onChange={handleChange}
              required
            />
            <input
              type="password"
              name="password"
              placeholder="Contraseña"
              value={form.password}
              onChange={handleChange}
              required
            />
            <button type="submit">Registrarse</button>
          </form>

          {error && <p style={{ color: 'red' }}>{error}</p>}
          {success && <p style={{ color: 'green' }}>{success}</p>}
        </div>
      </div>
    </div>
  );
};

export default Register;
