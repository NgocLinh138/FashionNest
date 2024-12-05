import api from '../api/api';

export const getAllCategories = async (filter = '', sortBy = '', sortOrderAscending = true, pageIndex = 1, pageSize = 10) => {
    try {
        const response = await api.get(`/category?Filter=${filter}&SortBy=${sortBy}&SortOrderAscending=${sortOrderAscending}&pageIndex=${pageIndex}&pageSize=${pageSize}`);
        return response.data.value.categories.items || [];
    } catch (error) {
        console.error('Error fetching categories:', error);
        throw error;
    }
};

export const getCategoryById = async (id) => {
    try {
        const response = await api.get(`/category/${id}`);
        return response.data;
    } catch (error) {
        console.error('Error fetching category by ID:', error);
        throw error;
    }
};

export const updateCategory = async (categoryData) => {
    try {
        await api.put('/category', categoryData);
    } catch (error) {
        console.error('Error updating category:', error);
        throw error;
    }
};

export const deleteCategory = async (categoryId) => {
    try {
        await api.delete(`/category/${categoryId}`);
    } catch (error) {
        console.error('Error deleting category:', error);
        throw error;
    }
};