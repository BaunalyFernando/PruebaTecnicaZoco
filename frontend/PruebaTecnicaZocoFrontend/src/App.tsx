import { BrowserRouter, Routes, Route } from 'react-router-dom';
import Login from './views/Login/login';
import Home from './views/home/home';
import Profile from './views/profile/Profile';
import Users from './views/users/Users';
import UserDetail from './views/usersDetail/UsersDetail';
import EditUser from './views/usersDetail/EditUser';
import EditAddress from './views/addresses/EditAddress';
import EditStudy from './views/studies/EditStudy';
import EditProfile from './views/profile/EditProfile';
import Register from './views/users/Register';
import RegisterUserAdmin from './views/users/RegisterUserAdmin'
import './App.css';

function App() {
  return (
    <BrowserRouter>
      <Routes>    
        <Route path="/login" element={<Login />} />
        <Route path="/register" element={<Register />} />
        <Route path="/registerAdmin" element={<RegisterUserAdmin />} />
        <Route path="/home" element={<Home />} />
        <Route path="/profile" element={<Profile />} />
        <Route path="/users" element={<Users />} />
        <Route path="/users/:id" element={<UserDetail />} />
        <Route path="/users/edit/:id" element={<EditUser />} />
        <Route path="/studies/:id/edit" element={<EditStudy />} />
        <Route path="/addresses/:id/edit" element={<EditAddress />} />
        <Route path="/profile/edit" element={<EditProfile />} />
      </Routes>
    </BrowserRouter>
  );
}

export default App;
