import api from '../api/api'; 

export const getAllUsers = async (filter = '', sortBy = '', sortOrderAscending = true) => {
  try {
    const response = await api.get(`/user?Filter=${filter}&SortBy=${sortBy}&SortOrderAscending=${sortOrderAscending}`);
    return response.data.value.users.items || []; 
  } catch (error) {
    console.error('Error fetching users:', error);
    throw error; 
  }
};

export const getUserById = async (userId) => {
  try {
    const response = await api.get(`/user/${userId}`);
    return response.data.user; 
  } catch (error) {
    console.error('Error fetching user by ID:', error);
    throw error;
  }
};

export const updateUserAPI = async (userData) => {
  try {
    const response = await api.put('/user', userData);
    return response.data; 
  } catch (error) {
    console.error('Error updating user:', error);
    throw error;
  }
};


export const deleteUser = async (userId) => {
  try {
    const response = await api.delete(`/user/${userId}`);
    return response.data;
  } catch (error) {
    console.error('Delete user error:', error.response || error.message);
    throw error; 
  }
};

