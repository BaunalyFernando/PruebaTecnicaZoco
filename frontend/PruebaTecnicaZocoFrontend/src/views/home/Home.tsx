import React, { useEffect, useState } from 'react';
import axios from 'axios';
import Navbar from '../../components/Navbar';
import styles from './Home.module.css';

const Home = () => {
  const [user, setUser] = useState(null);
  const [studies, setStudies] = useState([]);
  const [addresses, setAddresses] = useState([]);
  const token = sessionStorage.getItem('token');
  const userId = sessionStorage.getItem('userId');

  useEffect(() => {
    if (!token) {
      window.location.href = '/login';
      return;
    }

    const config = { headers: { Authorization: `Bearer ${token}` } };

    axios
      .get(`${import.meta.env.VITE_API_BASE_URL}/api/users/${userId}`, config)
      .then(res => {
        setUser(res.data);
        fetchStudies(config);
        fetchAddresses(config);
      })
      .catch(() => {
        window.location.href = '/login';
      });
  }, []);

  const fetchStudies = (config) => {
    axios
      .get(`${import.meta.env.VITE_API_BASE_URL}/api/studies/me`, config)
      .then(res => setStudies(res.data.$values))
      .catch(err => console.error('Error cargando estudios:', err));
  };

  const fetchAddresses = (config) => {
    axios
      .get(`${import.meta.env.VITE_API_BASE_URL}/api/addresses/me`, config)
      .then(res => setAddresses(res.data.$values))
      .catch(err => console.error('Error cargando direcciones:', err));
  };

  

  if (!user) return <div className={styles.container}>Cargando...</div>;

  return (
    <div className={styles.container}>
      <Navbar />
      <div className={styles.content}>
        <h1>¡Hola, {user.nombre}!</h1>
        <p><strong>Email:</strong> {user.email}</p>
        <p><strong>Dni:</strong> {user.dni}</p>
        <p><strong>Rol:</strong> {user.role === 0 ? "Administrador" : "Usuario"}</p>

        <h2>Estudios</h2>
        {studies.length === 0 ? (
          <p>No tienes estudios cargados.</p>
        ) : (
          <ul className={styles.list}>
            {studies.map((study) => (
              <li key={study.id}>
                {study.nombre} - {study.descripcion}
              </li>
            ))}
          </ul>
        )}

        <h2>Direcciones</h2>
        {addresses.length === 0 ? (
          <p>No tienes direcciones cargadas.</p>
        ) : (
          <ul className={styles.list}>
            {addresses.map((address) => (
              <li key={address.id}>
                {address.calle}, {address.ciudad}, {address.numero}
              </li>
            ))}
          </ul>
        )}
      </div>
    </div>
  );
};

export default Home;
