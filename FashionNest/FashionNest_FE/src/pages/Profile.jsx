import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { jwtDecode } from 'jwt-decode';
import { getUserById } from '../services/userService';
import ProfileLayout from '../components/ProfileLayout'; 

const Profile = () => {
  const [user, setUser] = useState(null);
  const navigate = useNavigate();

  useEffect(() => {
    const token = localStorage.getItem('authToken');
    if (!token) {
      navigate('/login');
    } else {
      try {
        const decodedToken = jwtDecode(token);
        const userToken = decodedToken['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'];
        const userId = userToken.match(/Value\s*=\s*(.+?)\s*}/)?.[1];

        if (userId) {
          getUserById(userId)
            .then((userData) => {
              setUser(userData);
            })
            .catch((error) => {
              console.error('Failed to fetch user:', error);
            });
        }
      } catch (error) {
        console.error('Error decoding token:', error);
      }
    }
  }, [navigate]);

  if (!user) {
    return (
      <div className="min-h-screen flex items-center justify-center bg-gray-100">
        <p className="text-gray-500 text-lg">Loading...</p>
      </div>
    );
  }

  return (
    <ProfileLayout>
      <h2 className="text-4xl font-bold text-gray-800 text-center mb-8">Thông tin cá nhân</h2>
      <div className="flex flex-col items-center gap-6">
        <div className="w-28 h-28 rounded-full bg-gradient-to-br from-indigo-500 to-purple-500 flex items-center justify-center text-white text-4xl font-bold shadow-xl">
          {user.name.charAt(0)}
        </div>
        <h3 className="text-2xl font-bold text-gray-700">{user.name}</h3>
        <p className="text-gray-500 text-lg">{user.email}</p>
      </div>
      <div className="mt-10 grid grid-cols-1 sm:grid-cols-2 gap-6">
          <div className="bg-gray-50 p-6 rounded-lg shadow-md flex flex-col items-start">
            <h3 className="text-lg font-semibold text-gray-700">Số điện thoại</h3>
            <p className="mt-2 text-gray-600">{user.phoneNumber || 'Chưa cập nhật'}</p>
          </div>
          <div className="bg-gray-50 p-6 rounded-lg shadow-md flex flex-col items-start">
            <h3 className="text-lg font-semibold text-gray-700">Địa chỉ</h3>
            <p className="mt-2 text-gray-600">{user.address || 'Chưa cập nhật'}</p>
          </div>
        </div>
        <div className="mt-10 text-center">
          <button
            onClick={() => navigate('/edit-profile')}
            className="bg-gradient-to-r from-pink-500 to-red-500 text-white text-lg py-3 px-8 rounded-lg hover:opacity-90 focus:outline-none focus:ring-4 focus:ring-pink-300 transition duration-300 shadow-md"
          >
            Chỉnh sửa thông tin
          </button>
        </div>
    </ProfileLayout>
  );
};

export default Profile;
