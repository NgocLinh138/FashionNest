import api from '../api/api'; 

export const updatePaymentStatus = async (orderId, status) => {
    try {
      const paymentData = {
        id: orderId,
        orderId: orderId,
        status: status, // 1: pending, 2: compeleted, 3: cancelled
      };
  
      const response = await api.put('/payment', paymentData);  
      return response.data;  
    } catch (error) {
      console.error('Error updating payment status:', error);
      throw error; 
    }
  };