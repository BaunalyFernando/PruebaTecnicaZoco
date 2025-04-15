import React, { useState } from 'react';
import axios from 'axios';
import styles from '../home/Home.module.css';
import Navbar from '../../components/Navbar';
import { useNavigate } from 'react-router-dom';

const RegisterUserAdmin = () => {
  const navigate = useNavigate();
  const [form, setForm] = useState({
    nombre: '',
    apellido: '',
    email: '',
    password: '',
    role: 0,
  });

  const [error, setError] = useState('');
  const [success, setSuccess] = useState('');

  const handleChange = (e) => {
    const { name, value } = e.target;
    setForm({
      ...form,
      [name]: value
    });
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    setError('');
    setSuccess('');

    const token = sessionStorage.getItem('token');

    const requestData = {
      id: 0,
      nombre: form.nombre,
      apellido: form.apellido,
      email: form.email,
      password: form.password,
      role: parseInt(form.role) 
    };

    axios.post(`${import.meta.env.VITE_API_BASE_URL}/api/Users/CreateUserByAdmin`, requestData, {
      headers: {
        Authorization: `Bearer ${token}`
      }
    })
    .then(() => {
      setSuccess('Usuario registrado correctamente.');
      setTimeout(() => navigate('/users'), 2000);
    })
    .catch((err) => {
      console.log(err.response);
      const msg = err.response?.data?.title || err.response?.data || 'Error al registrar usuario.';
      setError(typeof msg === 'string' ? msg : 'Error inesperado.');
    });
  };

  return (
    <div className={styles.container}>
      <Navbar />

      <div className={styles.content}>
        <div className={styles.section}>
          <h2>Crear Nuevo Usuario (Admin)</h2>
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
            <select
              name="role"
              value={form.role}
              onChange={handleChange}
              required
            >
              <option value={0}>Usuario</option>
              <option value={1}>Administrador</option>
            </select>

            <button type="submit">Crear Usuario</button>
          </form>

          {error && <p style={{ color: 'red' }}>{error}</p>}
          {success && <p style={{ color: 'green' }}>{success}</p>}
        </div>
      </div>
    </div>
  );
};

export default RegisterUserAdmin;