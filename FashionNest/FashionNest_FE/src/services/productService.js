import api from '../api/api';

export const getAllProducts = async (filter = '', sortBy = '', sortOrderAscending = true, pageIndex = 1, pageSize = 10) => {
  try {
    const response = await api.get(`/product?Filter=${filter}&SortBy=${sortBy}&SortOrderAscending=${sortOrderAscending}&pageIndex=${pageIndex}&pageSize=${pageSize}`);
    return response.data.value.products.items || [];
  } catch (error) {
    console.error('Error fetching products:', error);
    throw error;
  }
};

export const getProductById = async (id) => {
  try {
    const response = await api.get(`/product/${id}`);
    return response.data; 
  } catch (error) {
    console.error('Lỗi khi lấy dữ liệu người dùng:', error);
    throw error;  
  }
};

export const updateProduct = async (productData) => {
  try {
    await api.put('/product', productData);
  } catch (error) {
    console.error('Error updating product:', error);
    throw error;
  }
};


export const deleteProduct = async (productId) => {
  try {
    await api.delete(`/product/${productId}`);
  } catch (error) {
    console.error('Error deleting product:', error);
    throw error;
  }
};

