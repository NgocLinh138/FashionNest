import api from '../api/api';

export const login = async (email, password) => {
  try {
    const response = await api.post('/auth/login', { email, password });
    console.log("Response từ API:", response);
    return response.data.user || response.data; 
  } catch (error) {
    console.error("Lỗi API:", error.response || error.message);
    throw error;
  }
};


// API đăng ký
export const register = async (name, email, password, confirmPassword, phoneNumber, address) => {
  try {
    const response = await api.post('/auth/register', {
      name,
      email,
      password,
      confirmPassword,
      phoneNumber,
      address,
    });
    return response.data;
  } catch (error) {
    console.error(error);
    throw error;
  }
};


export const changePassword = async (email, oldPassword, newPassword, confirmPassword) => {
  if (newPassword !== confirmPassword) {
    throw new Error('Mật khẩu xác nhận không khớp!');
  }

  const token = localStorage.getItem('authToken');
  if (!token) {
    throw new Error('Không tìm thấy token xác thực.');
  }

  try {
    const response = await api.post('/auth/change-password', {
      email,
      oldPassword,
      newPassword,
      confirmPassword
    }, {
      headers: {
        Authorization: `Bearer ${token}`  
      }
    });
    console.log('Password change response:', response);
    return response.data; 
  } catch (error) {
    console.error('Error changing password:', error.response || error.message);
    throw error;
  }
}