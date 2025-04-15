import { Link } from 'react-router-dom';
import styles from '../views/home/Home.module.css';
import axios from 'axios';

const Navbar = () => {
  const role = sessionStorage.getItem('role');
  const userId = sessionStorage.getItem('userId');
  const token = sessionStorage.getItem('token');

  const handleLogout = () => {
    axios
      .post(
        `${import.meta.env.VITE_API_BASE_URL}/auth/logout`,
        { userId: parseInt(userId) },
        {
          headers: {
            Authorization: `Bearer ${token}`,
            'Content-Type': 'application/json'
          }
        }
      )
      .finally(() => {
        sessionStorage.removeItem('token');
        sessionStorage.removeItem('userId');
        sessionStorage.removeItem('role');
        window.location.href = '/login';
      });
  };

  return (
    <nav className={styles.navbar}>
      <div className={styles.navLeft}>
        <Link to="/home" className={styles.navLink}>Inicio</Link>
        {role === 'Admin' && <Link to="/users" className={styles.navLink}>Usuarios</Link>}
        <Link to="/profile" className={styles.navLink}>Perfil</Link>
      </div>
      <div className={styles.navRight}>
        <button onClick={handleLogout} className={styles.logoutButton}>Cerrar sesión</button>
      </div>
    </nav>
  );
};

export default Navbar;
