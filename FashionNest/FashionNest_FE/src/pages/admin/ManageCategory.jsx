import React, { useState, useEffect } from 'react';
import { getAllCategories, updateCategory, deleteCategory } from '../../services/categoryService';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faSortUp, faSortDown, faArrowRight } from '@fortawesome/free-solid-svg-icons';
import Swal from 'sweetalert2';
import DataTable from '../../components/DataTable';

const ManageCategory = () => {
    const [categories, setCategories] = useState([]);
    const [loading, setLoading] = useState(true);
    const [searchTerm, setSearchTerm] = useState('');
    const [sortBy, setSortBy] = useState('price');
    const [sortOrderAscending, setSortOrderAscending] = useState(true);
    const [isModalOpen, setIsModalOpen] = useState(false);
    const [isUpdateModalOpen, setIsUpdateModalOpen] = useState(false);
    const [selectedCategory, setSelectedCategory] = useState(null);
    const [updateCategoryData, setUpdateCategoryData] = useState(null);

    useEffect(() => {
        const fetchCategories = async () => {
            try {
                const fetchedCategories = await getAllCategories(searchTerm, sortBy, sortOrderAscending);
                setCategories(fetchedCategories);
                setLoading(false);
            } catch (error) {
                console.error('Failed to fetch category data:', error);
                setLoading(false);
            }
        };

        fetchCategories();
    }, [searchTerm, sortBy, sortOrderAscending]);

    const handleSearchChange = (event) => {
        setSearchTerm(event.target.value);
    };

    const handleSortChange = (field) => {
        if (sortBy === field) {
            setSortOrderAscending(!sortOrderAscending);
        } else {
            setSortBy(field);
            setSortOrderAscending(true);
        }
    };

    const columns = [
        { field: 'id', label: 'ID' },
        { field: 'name', label: 'Name', sortable: true },
        { field: 'description', label: 'Description' },
        {
            field: 'view',
            label: 'View',
            render: (row) => (
                <span
                    onClick={() => openModal(row)}
                    className="cursor-pointer"
                >
                    <FontAwesomeIcon icon={faArrowRight} className="text-xl" />
                </span>
            ),
        },
    ];

    const openModal = (category) => {
        setSelectedCategory(category);
        setIsModalOpen(true);
    };

    const closeModal = () => {
        setIsModalOpen(false);
        setSelectedCategory(null);
    };

    const openUpdateModal = (category) => {
        setUpdateCategoryData({ ...category });
        setIsUpdateModalOpen(true);
    };

    const closeUpdateModal = () => {
        setIsUpdateModalOpen(false);
        setUpdateCategoryData(null);
    };

    const handleUpdate = async () => {
        try {
            const updatedCategoryData = {
                id: updateCategoryData.id,
                name: updateCategoryData.name,
                description: updateCategoryData.description,
            };

            await updateCategory(updatedCategoryData);
            Swal.fire('Success!', 'Category updated successfully.', 'success');
            setIsModalOpen(false);

            const fetchedCategories = await getAllCategories(searchTerm, sortBy, sortOrderAscending);
            setCategories(fetchedCategories);
        } catch (error) {
            Swal.fire('Error!', 'There was an issue updating the category.', 'error');
        }
    };

    const handleDelete = async () => {
        const result = await Swal.fire({
            title: 'Are you sure?',
            text: "This action cannot be undone!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonText: 'Yes, delete it!',
            cancelButtonText: 'Cancel',
        });

        if (result.isConfirmed) {
            try {
                await deleteCategory(selectedCategory.id);
                setCategories(categories.filter((category) => category.id !== selectedCategory.id));
                closeModal();
                Swal.fire({
                    title: 'Thành công!',
                    text: 'Danh mục đã được xóa.',
                    icon: 'success',
                    showConfirmButton: false,
                    showCancelButton: false,
                    background: '#fff',
                    timer: 1000
                })
            } catch (error) {
                Swal.fire('Error!', 'There was an issue deleting the category.', 'error');
            }
        }
    };

    if (loading) {
        return <div className="text-center py-6">Loading data...</div>;
    }


    return (
        <div className="p-6 bg-white rounded-lg shadow-md">
            <h2 className="text-2xl font-semibold mb-6">Manage Categories</h2>

            <div className="mb-4">
                <input
                    type="text"
                    placeholder="Search categories..."
                    value={searchTerm}
                    onChange={handleSearchChange}
                    className="w-full p-2 border rounded-lg"
                />
            </div>

            <DataTable
                data={categories}
                columns={columns}
                sortBy={sortBy}
                sortOrderAscending={sortOrderAscending}
                onSortChange={handleSortChange}
                onRowClick={() => { }}
            />

            {isModalOpen && selectedCategory && (
                <div className="fixed inset-0 flex items-center justify-center bg-black bg-opacity-50">
                    <div className="bg-white p-6 rounded-lg w-96 shadow-lg">
                        <h3 className="text-xl font-semibold mb-6 text-center text-gray-800">Category Details</h3>
                        <div className="space-y-4">
                            <p><strong>ID:</strong> {selectedCategory.id}</p>
                            <p><strong>Name:</strong> {selectedCategory.name}</p>
                            <p><strong>Description:</strong> {selectedCategory.description}</p>
                        </div>
                        <div className="mt-6 flex justify-between gap-4">
                            <button
                                onClick={() => openUpdateModal(selectedCategory)}
                                className="px-4 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700"
                            >
                                Update
                            </button>
                            <button
                                onClick={handleDelete}
                                className="px-4 py-2 bg-red-600 text-white rounded-lg hover:bg-red-700"
                            >
                                Delete
                            </button>
                            <button
                                onClick={closeModal}
                                className="px-4 py-2 bg-gray-500 text-white rounded-lg hover:bg-gray-600"
                            >
                                Close
                            </button>
                        </div>
                    </div>
                </div>
            )}

            {isModalOpen && updateCategoryData && (
                <div className="fixed inset-0 flex items-center justify-center bg-black bg-opacity-50">
                    <div className="bg-white p-6 rounded-lg w-96 shadow-lg">
                        <h3 className="text-xl font-semibold mb-6 text-center text-gray-800">Update Category</h3>
                        <div className="space-y-4">
                            <div>
                                <label className="block text-gray-700">Name</label>
                                <input
                                    type="text"
                                    value={updateCategoryData.name}
                                    onChange={(e) => setUpdateCategoryData({ ...updateCategoryData, name: e.target.value })}
                                    className="w-full p-2 border rounded"
                                />
                            </div>
                            <div>
                                <label className="block text-gray-700">Description</label>
                                <textarea
                                    value={updateCategoryData.description}
                                    onChange={(e) => setUpdateCategoryData({ ...updateCategoryData, description: e.target.value })}
                                    className="w-full p-2 border rounded"
                                ></textarea>
                            </div>
                        </div>
                        <div className="mt-6 flex justify-between gap-4">
                            <button
                                onClick={handleUpdate}
                                className="px-4 py-2 bg-green-600 text-white rounded-lg hover:bg-green-700"
                            >
                                Save
                            </button>
                            <button
                                onClick={closeUpdateModal}
                                className="px-4 py-2 bg-gray-500 text-white rounded-lg hover:bg-gray-600"
                            >
                                Cancel
                            </button>
                        </div>
                    </div>
                </div>
            )}
        </div>
    );
};

export default ManageCategory;
