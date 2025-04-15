import React, { useEffect, useState } from 'react';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';
import styles from '../home/Home.module.css';
import Navbar from '../../components/Navbar';

const EditProfile = () => {
  const [userData, setUserData] = useState({ nombre: '', apellido: '', email: '', password: '' });
  const token = sessionStorage.getItem('token');
  const userId = sessionStorage.getItem('userId');
  const navigate = useNavigate();

  useEffect(() => {
    if (!token) {
      navigate('/login');
      return;
    }

    const config = { headers: { Authorization: `Bearer ${token}` } };

    axios
      .get(`${import.meta.env.VITE_API_BASE_URL}/api/users/${userId}`, config)
      .then(res => {
        setUserData({
          nombre: res.data.nombre || '',
          apellido: res.data.apellido || '',
          email: res.data.email || '',
          password: res.data.password || '',
        });
      })
      .catch(err => console.error(err));
  }, []);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setUserData(prev => ({ ...prev, [name]: value }));
  };

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    const config = { headers: { Authorization: `Bearer ${token}` } };
  
    const payload = {
      id: Number(userId),
      ...userData,
    };
  
    axios
      .put(`${import.meta.env.VITE_API_BASE_URL}/api/users`, payload, config)
      .then(() => navigate('/profile'))
      .catch(err => console.error(err));
  };

  return (
    <div className={styles.container}>
      <Navbar />
      <div className={styles.content}>
        <h2>Editar Perfil</h2>
        <form onSubmit={handleSubmit} className={styles.form}>
          <label>Nombre:</label>
          <input
            type="text"
            name="nombre"
            value={userData.nombre}
            onChange={handleChange}
            required
          />

          <label>Apellido:</label>
          <input
            type="text"
            name="apellido"
            value={userData.apellido}
            onChange={handleChange}
            required
          />

          <label>Email:</label>
          <input
            type="email"
            name="email"
            value={userData.email}
            onChange={handleChange}
            required
          />

          <label>Password:</label>
          <input
            type="password"
            name="password"
            value={userData.password}
            onChange={handleChange}
            required
          />

          <button type="submit" className={styles.editButton}>Guardar cambios</button>
        </form>
      </div>
    </div>
  );
};

export default EditProfile;
