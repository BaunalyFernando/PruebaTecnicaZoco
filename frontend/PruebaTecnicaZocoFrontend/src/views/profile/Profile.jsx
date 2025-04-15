import React, { useEffect, useState } from 'react';
import axios from 'axios';
import styles from '../home/Home.module.css';
import Navbar from '../../components/Navbar';
import { Link, useNavigate } from 'react-router-dom';

const Profile = () => {
  const [user, setUser] = useState(null);
  const [addresses, setAddresses] = useState([]);
  const [studies, setStudies] = useState([]);
  const [showAddressForm, setShowAddressForm] = useState(false);
  const [showStudyForm, setShowStudyForm] = useState(false);
  const [newAddress, setNewAddress] = useState({
    calle: '',
    numero: '',
    ciudad: '',
    userId: 0
  });
  const [newStudy, setNewStudy] = useState({
    nombre: '',
    descripcion: '',
    userId: 0
  });
  const [error, setError] = useState('');
  const [success, setSuccess] = useState('');

  const navigate = useNavigate();
  const token = sessionStorage.getItem('token');
  const userId = sessionStorage.getItem('userId');

  const fetchData = () => {
    if (!token) {
      navigate('/login');
      return;
    }

    const config = { headers: { Authorization: `Bearer ${token}` } };

    axios.get(`${import.meta.env.VITE_API_BASE_URL}/api/users/${userId}`, config)
      .then(res => setUser(res.data))
      .catch(() => navigate('/login'));

    axios.get(`${import.meta.env.VITE_API_BASE_URL}/api/Addresses/${userId}`, config)
      .then(res => setAddresses(res.data.$values || []))
      .catch(console.error);

    axios.get(`${import.meta.env.VITE_API_BASE_URL}/api/Studies/${userId}`, config)
      .then(res => setStudies(res.data.$values || []))
      .catch(console.error);
  };

  useEffect(() => {
    fetchData();
  }, []);

  const handleAddressChange = (e) => {
    setNewAddress({
      ...newAddress,
      [e.target.name]: e.target.value
    });
  };

  const handleStudyChange = (e) => {
    setNewStudy({
      ...newStudy,
      [e.target.name]: e.target.value
    });
  };

  const handleDeleteAddress = (id) => {
    const confirmDelete = window.confirm('¿Estás seguro de que deseas eliminar esta dirección?');
    if (!confirmDelete) return;

    const config = { headers: { Authorization: `Bearer ${token}` } };

    axios.delete(`${import.meta.env.VITE_API_BASE_URL}/api/Addresses/${id}`, config)
      .then(() => {
        setSuccess('Dirección eliminada correctamente');
        fetchData();
      })
      .catch((err) => {
        console.error(err);
        setError('Error al eliminar la dirección');
      });
  };

  const handleDeleteStudy = (id) => {
    const confirmDelete = window.confirm('¿Estás seguro de que deseas eliminar este estudio?');
    if (!confirmDelete) return;

    const config = { headers: { Authorization: `Bearer ${token}` } };

    axios.delete(`${import.meta.env.VITE_API_BASE_URL}/api/Studies/${id}`, config)
      .then(() => {
        setSuccess('Estudio eliminado correctamente');
        fetchData();
      })
      .catch((err) => {
        console.error(err);
        setError('Error al eliminar el estudio');
      });
  };



  const createAddress = (e) => {
    e.preventDefault();
    setError('');
    setSuccess('');

    const config = { headers: { Authorization: `Bearer ${token}` } };
    const addressData = {
      ...newAddress,
      userId: parseInt(userId)
    };

    axios.post(`${import.meta.env.VITE_API_BASE_URL}/api/Addresses`, addressData, config)
      .then(() => {
        setSuccess('Dirección agregada correctamente');
        setNewAddress({ calle: '', numero: '', ciudad: '', userId: 0 });
        setShowAddressForm(false);
        fetchData();
      })
      .catch(err => {
        console.error(err);
        setError('Error al crear la dirección');
      });
  };

  const createStudy = (e) => {
    e.preventDefault();
    setError('');
    setSuccess('');

    const config = { headers: { Authorization: `Bearer ${token}` } };
    const studyData = {
      ...newStudy,
      userId: parseInt(userId)
    };

    axios.post(`${import.meta.env.VITE_API_BASE_URL}/api/Studies`, studyData, config)
      .then(() => {
        setSuccess('Estudio agregado correctamente');
        setNewStudy({ nombre: '', descripcion: '', userId: 0 });
        setShowStudyForm(false);
        fetchData();
      })
      .catch(err => {
        console.error(err);
        setError('Error al crear el estudio');
      });
  };

  if (!user) return <div className={styles.container}>Cargando...</div>;

  return (
    <div className={styles.container}>
      <Navbar />
      <div className={styles.content}>
        <h1>Perfil de {user.nombre}</h1>
        <ul className={styles.list}>
          <li><strong>Email:</strong> {user.email}</li>
          <li><strong>Rol:</strong> {user.role === 0 ? 'Administrador' : 'Usuario'}</li>
        </ul>
        <Link to={`/profile/edit`}>
          <button className={styles.editButton}>Editar datos personales</button>
        </Link>

        <div className={styles.section}>
          <div className={styles.sectionHeader}>
            <h2>Direcciones</h2>
            <button
              className={styles.addButton}
              onClick={() => setShowAddressForm(!showAddressForm)}
            >
              {showAddressForm ? 'Cancelar' : 'Agregar dirección'}
            </button>
          </div>

          {showAddressForm && (
            <form onSubmit={createAddress} className={styles.form}>
              <input
                type="text"
                name="calle"
                placeholder="Calle"
                value={newAddress.calle}
                onChange={handleAddressChange}
                required
              />
              <input
                type="text"
                name="numero"
                placeholder="Número"
                value={newAddress.numero}
                onChange={handleAddressChange}
                required
              />
              <input
                type="text"
                name="ciudad"
                placeholder="Ciudad"
                value={newAddress.ciudad}
                onChange={handleAddressChange}
                required
              />
              <button type="submit">Guardar dirección</button>
            </form>
          )}

          <ul className={styles.list}>
            {addresses.length > 0 ? (
              addresses.map(addr => (
                <li key={addr.id} className={styles.listItem}>
                  {addr.calle} {addr.numero}, {addr.ciudad}
                  <Link to={`/addresses/${addr.id}/edit`}>
                    <button className={styles.editButton}>Editar</button>
                  </Link>
                  <button
                    className={styles.deleteButton}
                    onClick={() => handleDeleteAddress(addr.id)}
                  >
                    Eliminar
                  </button>
                </li>
              ))
            ) : (
              <li>No hay direcciones registradas</li>
            )}

          </ul>
        </div>

        <div className={styles.section}>
          <div className={styles.sectionHeader}>
            <h2>Estudios</h2>
            <button
              className={styles.addButton}
              onClick={() => setShowStudyForm(!showStudyForm)}
            >
              {showStudyForm ? 'Cancelar' : 'Agregar estudio'}
            </button>
          </div>

          {showStudyForm && (
            <form onSubmit={createStudy} className={styles.form}>
              <input
                type="text"
                name="nombre"
                placeholder="Nombre del estudio"
                value={newStudy.nombre}
                onChange={handleStudyChange}
                required
              />
              <textarea
                name="descripcion"
                placeholder="Descripción"
                value={newStudy.descripcion}
                onChange={handleStudyChange}
                required
              />
              <button type="submit">Guardar estudio</button>
            </form>
          )}

          <ul className={styles.list}>
            {studies.length > 0 ? (
              studies.map(study => (
                <li key={study.id} className={styles.listItem}>
                  <strong>{study.nombre}:</strong> {study.descripcion}
                  <Link to={`/studies/${study.id}/edit`}>
                    <button className={styles.editButton}>Editar</button>
                  </Link>
                  <button
                    className={styles.deleteButton}
                    onClick={() => handleDeleteStudy(study.id)}
                  >
                    Eliminar
                  </button>
                </li>
              ))
            ) : (
              <li>No hay estudios registrados</li>
            )}

          </ul>
        </div>

        {error && <p className={styles.errorMessage}>{error}</p>}
        {success && <p className={styles.successMessage}>{success}</p>}
      </div>
    </div>
  );
};

export default Profile;