import React, { useEffect, useState } from 'react';
import axios from 'axios';

const Home = () => {
  const [user, setUser] = useState(null);
  const [studies, setStudies] = useState([]);
  const [addresses, setAddresses] = useState([]);
  const token = localStorage.getItem('token');
    const userId = localStorage.getItem('userId');

  useEffect(() => {
    
    console.log('TOKEN:', token);
    console.log('USER ID:', userId);
  
    if (!token) {
      console.warn('No hay token, redirigiendo a login');
      window.location.href = '/login';
      return;
    }
  
    const config = {
      headers: { Authorization: `Bearer ${token}` }
    };
  
    axios
      .get(`http://localhost:5186/api/users/${userId}`, config)
      .then(res => {
        console.log('Usuario recibido:', res.data);
        setUser(res.data);
        fetchStudies(config);
        fetchAddresses(config);
      })
      .catch(err => {
        console.error('Error al obtener el usuario:', err.response?.status, err.response?.data || err);
        window.location.href = '/login';
      });
  }, []);
  

  const fetchStudies = (config) => {
    axios
      .get(`http://localhost:5186/api/studies/me`, config)
      .then(res => setStudies(res.data.$values))
      .catch(err => console.error('Error cargando estudios:', err));
  };

  const fetchAddresses = (config) => {
    axios
      .get('http://localhost:5186/api/addresses/me', config)
      .then(res => setAddresses(res.data.$values))
      .catch(err => console.error('Error cargando direcciones:', err));
  };

  const logout = () => {
    axios
      .post(
        'http://localhost:5186/auth/logout',
        { userId: parseInt(userId) },
        {
          headers: {
            Authorization: `Bearer ${token}`,
            'Content-Type': 'application/json'
          }
        }
      )
      .finally(() => {
        localStorage.removeItem('token');
        localStorage.removeItem('userId');
        localStorage.removeItem('role');
        window.location.href = '/login';
      });
  };

  if (!user) return <div>Cargando...</div>;

  return (
    <div style={{ padding: '20px' }}>
      <h1>¡Hola, {user.nombre}!</h1>
      <p><strong>Email:</strong> {user.email}</p>
      <p><strong>Rol:</strong> {user.role === 0 ? "Usuario" : "Administrador"}</p>

      <h2>Estudios</h2>
      {studies.length === 0 ? (
        <p>No tienes estudios cargados.</p>
      ) : (
        <ul>
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
        <ul>
          {addresses.map((address) => (
            <li key={address.id}>
              {address.calle}, {address.ciudad}, {address.numero}
            </li>
          ))}
        </ul>
      )}

      <button onClick={logout} style={{ marginTop: '20px' }}>
        Cerrar sesión
      </button>
    </div>
  );
};

export default Home;
