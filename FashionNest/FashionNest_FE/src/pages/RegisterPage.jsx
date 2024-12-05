import React, { useState } from 'react';
import RegisterForm from '../components/RegisterForm';

const RegisterPage = () => {
  const [userData, setUserData] = useState(null);

  const handleRegisterSuccess = (data) => {
    setUserData(data);
  };

  return (
    <div className="h-screen bg-white flex items-center justify-center py-10 px-6 sm:px-8 lg:px-10 pt-24">
      <div className="max-w-xl w-full bg-white p-8 rounded-xl shadow-lg">
        {!userData ? (
          <RegisterForm onRegisterSuccess={handleRegisterSuccess} />
        ) : (
          <div className="text-center">
            <h2 className="text-2xl font-semibold text-teal-600">Đăng ký thành công!</h2>
            <p className="text-lg text-gray-600 mt-2">Chào, {userData.user.name}!</p>
          </div>
        )}
      </div>
    </div>
  );
};

export default RegisterPage;
