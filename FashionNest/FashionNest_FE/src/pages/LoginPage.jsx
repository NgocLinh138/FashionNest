import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import LoginForm from '../components/LoginForm';

const LoginPage = () => {
  const [userData, setUserData] = useState(null);
  const navigate = useNavigate();

  const handleLoginSuccess = (data) => {
    const { token, role } = data;
    if (token) {
      console.log('Đã nhận được token:', token);
      localStorage.setItem('authToken', token);
      localStorage.setItem('userRole', role);

      if (role === 'Admin') {
        navigate('/admin/dashboard');
      } else if (role === 'Customer') {
        navigate('/');
      }

      setUserData(data);
    } else {
      console.error('Không có token trong dữ liệu đăng nhập');
    }
  };

  return (
    <div className="min-h-screen bg-white flex items-center justify-center py-10 px-6 sm:px-8 lg:px-10 overflow-hidden">
      <div className="max-w-xl w-full bg-white p-8 rounded-xl shadow-lg">
        {!userData ? (
          <LoginForm onLoginSuccess={handleLoginSuccess} />
        ) : (
          <div className="text-center">
            <h2 className="text-2xl font-semibold text-teal-600">Đăng nhập thành công!</h2>
            <p className="text-lg text-gray-600 mt-2">Chào, {userData.user.name}!</p>
          </div>
        )}
      </div>
    </div>
  );
};

export default LoginPage;
