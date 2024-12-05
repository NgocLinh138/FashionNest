import api from '../api/api';

export const createOrder = async (orderData) => {
  try {
    const response = await api.post('/order', orderData);
    return response.data.order;
  } catch (error) {
    console.error('Error creating order:', error);
    throw error;
  }
};

export const getOrderById = async (orderId) => {
  try {
    const response = await api.get(`/order/${orderId}`);
    return response.data.order;
  } catch (error) {
    console.error('Error fetching order by ID:', error);
    throw error;
  }
};

export const getOrdersByUserId = async (userId) => {
  try {
    const response = await api.get(`/order?UserId=${userId}`);
    return response.data.value.orders.items || [];
  } catch (error) {
    console.error('Error fetching orders by userId:', error);
    throw error;
  }
};

export const updateOrderStatus = async (orderId, status) => {
  try {
    const response = await api.put(`/order/${orderId}`,
      {
        orderId: orderId,
        status: status
      });
    return response.data.order;
  } catch (error) {
    console.error('Error updating order status:', error);
    throw error;
  }
};

export const getAllOrders = async (filter = '', sortBy = '', sortOrderAscending = true) => {
  try {
    const response = await api.get(`/order?Filter=${filter}&SortBy=${sortBy}&SortOrderAscending=${sortOrderAscending}`);
    return response.data.value.orders.items || [];
  } catch (error) {
    console.error('Error fetching orders:', error);
    throw error;
  }
};

export const deleteOrder = async (orderId) => {
  try {
    const response = await api.delete(`/order/${orderId}`);
    return response.data;
  } catch (error) {
    console.error('Error deleting order:', error);
    throw error;
  }
};