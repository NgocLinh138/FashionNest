import React, { useState, useEffect } from 'react';
import { getAllProducts, deleteProduct, updateProduct } from '../../services/productService';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faSortUp, faSortDown, faArrowRight } from '@fortawesome/free-solid-svg-icons';
import Swal from 'sweetalert2';
import DataTable from '../../components/DataTable';

const ManageProduct = () => {
  const [products, setProducts] = useState([]);
  const [loading, setLoading] = useState(true);
  const [searchTerm, setSearchTerm] = useState('');
  const [sortBy, setSortBy] = useState('price');
  const [sortOrderAscending, setSortOrderAscending] = useState(true);
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [isUpdateModalOpen, setIsUpdateModalOpen] = useState(false);
  const [selectedProduct, setSelectedProduct] = useState(null);
  const [updateProductData, setUpdateProductData] = useState(null);
  const [isAddModalOpen, setIsAddModalOpen] = useState(false);
  const [newProduct, setNewProduct] = useState({
    name: '',
    price: '',
    description: '',
    stock: '',
    category: '',
  });

  useEffect(() => {
    const fetchProducts = async () => {
      try {
        const fetchedProducts = await getAllProducts(searchTerm, sortBy, sortOrderAscending);
        setProducts(fetchedProducts);
        setLoading(false);
      } catch (error) {
        console.error('Failed to fetch product data:', error);
        setLoading(false);
      }
    };

    fetchProducts();
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
    { field: 'name', label: 'Name' },
    { field: 'description', label: 'Description' },
    { field: 'price', label: 'Price', sortable: true },
    { field: 'stock', label: 'Stock' },
    {
      field: 'category.name',
      label: 'Category',
      render: (row) => row.category?.name || 'No Category'
    },
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

  const openModal = (product) => {
    setSelectedProduct(product);
    setIsModalOpen(true);
  };

  const closeModal = () => {
    setIsModalOpen(false);
    setSelectedProduct(null);
  };

  const openUpdateModal = (product) => {
    setUpdateProductData({ ...product });
    setIsUpdateModalOpen(true);
  };

  const closeUpdateModal = () => {
    setIsUpdateModalOpen(false);
    setUpdateProductData(null);
  };

  const openAddModal = () => {
    setIsAddModalOpen(true);
  };

  const closeAddModal = () => {
    setIsAddModalOpen(false);
    setNewProduct({
      name: '',
      price: '',
      description: '',
      stock: '',
      category: '',
    });
  };

  const handleAddProduct = async () => {
    try {
      const productData = { ...newProduct };
      await addProduct(productData);
      Swal.fire({
        title: 'Success!',
        text: 'New product added successfully.',
        icon: 'success',
        showConfirmButton: false,
        showCancelButton: false,
        background: '#fff',
        timer: 1000,
      });
      closeAddModal();
      const fetchedProducts = await getAllProducts(searchTerm, sortBy, sortOrderAscending);
      setProducts(fetchedProducts);
    } catch (error) {
      Swal.fire('Error!', 'There was an issue adding the product.', 'error');
    }
  };

  const handleUpdate = async () => {
    try {
      const updatedProductData = {
        productId: updateProductData.id,
        name: updateProductData.name,
        price: updateProductData.price,
        description: updateProductData.description,
        stock: updateProductData.stock,
        category: updateProductData.category,
      };

      await updateProduct(updatedProductData);
      Swal.fire({
        title: 'Thành công!',
        text: 'Chi tiết sản phẩm được cập nhật thành công.',
        icon: 'success',
        showConfirmButton: false,
        showCancelButton: false,
        background: '#fff',
        timer: 1000
      })
      setIsUpdateModalOpen(false);

      const fetchedProducts = await getAllProducts(searchTerm, sortBy, sortOrderAscending);
      setProducts(fetchedProducts);
    } catch (error) {
      Swal.fire('Error!', 'There was an issue updating the product.', 'error');
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
        await deleteProduct(selectedProduct.id);
        setProducts(products.filter((product) => product.id !== selectedProduct.id));
        closeModal();
        Swal.fire({
          title: 'Thành công!',
          text: 'Sản phẩm đã được xóa.',
          icon: 'success',
          showConfirmButton: false,
          showCancelButton: false,
          background: '#fff',
          timer: 1000
        })
      } catch (error) {
        Swal.fire('Error!', 'There was an issue deleting the product.', 'error');
      }
    }
  };

  if (loading) {
    return <div className="text-center py-6">Loading data...</div>;
  }

  return (
    <div className="p-6 bg-white rounded-lg shadow-md">
      <h2 className="text-2xl font-semibold mb-6">Manage Products</h2>

      <div className="mb-4">
        <input
          type="text"
          placeholder="Search products..."
          value={searchTerm}
          onChange={handleSearchChange}
          className="w-full p-2 border rounded-lg"
        />
      </div>

      <button
        onClick={openAddModal}
        className="mb-4 px-4 py-2 bg-green-600 text-white rounded-lg hover:bg-green-700"
      >
        Add Product
      </button>

      <DataTable
        data={products}
        columns={columns}
        sortBy={sortBy}
        sortOrderAscending={sortOrderAscending}
        onSortChange={handleSortChange}
        onRowClick={() => { }}
      />

      {isModalOpen && selectedProduct && (
        <div className="fixed inset-0 flex items-center justify-center bg-black bg-opacity-50">
          <div className="bg-white p-6 rounded-lg w-96 shadow-lg transform transition-all scale-100 hover:scale-105">
            <h3 className="text-xl font-semibold mb-6 text-center text-gray-800">Product Details</h3>

            <div className="space-y-4">
              <p><strong>ID:</strong> {selectedProduct.id}</p>
              <p><strong>Name:</strong> {selectedProduct.name}</p>
              <p><strong>Description:</strong> {selectedProduct.description}</p>
              <p><strong>Price:</strong> {selectedProduct.price}</p>
              <p><strong>Stock:</strong> {selectedProduct.stock}</p>
              <p><strong>Category:</strong> {selectedProduct.category.name}</p>
            </div>

            <div className="mt-6 flex justify-between gap-4">
              <button
                onClick={() => openUpdateModal(selectedProduct)}
                className="px-4 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 focus:outline-none focus:ring-2 focus:ring-blue-300 transition"
              >
                Update
              </button>
              <button
                onClick={handleDelete}
                className="px-4 py-2 bg-red-600 text-white rounded-lg hover:bg-red-700 focus:outline-none focus:ring-2 focus:ring-red-300 transition"
              >
                Delete
              </button>
              <button
                onClick={closeModal}
                className="px-4 py-2 bg-gray-500 text-white rounded-lg hover:bg-gray-600 focus:outline-none focus:ring-2 focus:ring-gray-300 transition"
              >
                Close
              </button>
            </div>
          </div>
        </div>
      )}

      {isAddModalOpen && (
        <div className="fixed inset-0 flex items-center justify-center bg-black bg-opacity-50">
          <div className="bg-white p-6 rounded-lg w-96 shadow-lg">
            <h3 className="text-xl font-semibold mb-6 text-center text-gray-800">Add New Product</h3>
            <div className="space-y-4">
              <div>
                <label className="block text-gray-700">Name</label>
                <input
                  type="text"
                  value={newProduct.name}
                  onChange={(e) => setNewProduct({ ...newProduct, name: e.target.value })}
                  className="w-full p-2 border rounded"
                />
              </div>
              <div>
                <label className="block text-gray-700">Price</label>
                <input
                  type="number"
                  value={newProduct.price}
                  onChange={(e) => setNewProduct({ ...newProduct, price: e.target.value })}
                  className="w-full p-2 border rounded"
                />
              </div>
              <div>
                <label className="block text-gray-700">Description</label>
                <textarea
                  value={newProduct.description}
                  onChange={(e) => setNewProduct({ ...newProduct, description: e.target.value })}
                  className="w-full p-2 border rounded"
                ></textarea>
              </div>
              <div>
                <label className="block text-gray-700">Stock</label>
                <input
                  type="number"
                  value={newProduct.stock}
                  onChange={(e) => setNewProduct({ ...newProduct, stock: e.target.value })}
                  className="w-full p-2 border rounded"
                />
              </div>
              <div>
                <label className="block text-gray-700">Category</label>
                <input
                  type="text"
                  value={newProduct.category}
                  onChange={(e) => setNewProduct({ ...newProduct, category: e.target.value })}
                  className="w-full p-2 border rounded"
                />
              </div>
            </div>
            <div className="mt-6 flex justify-between gap-4">
              <button
                onClick={handleAddProduct}
                className="px-4 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700"
              >
                Add Product
              </button>
              <button
                onClick={closeAddModal}
                className="px-4 py-2 bg-gray-500 text-white rounded-lg hover:bg-gray-600"
              >
                Cancel
              </button>
            </div>
          </div>
        </div>
      )}

      {isUpdateModalOpen && updateProductData && (
        <div className="fixed inset-0 flex items-center justify-center bg-black bg-opacity-50">
          <div className="bg-white p-6 rounded-lg w-96 shadow-lg">
            <h3 className="text-xl font-semibold mb-6 text-center text-gray-800">Update Product</h3>
            <div className="space-y-4">
              <div>
                <label className="block text-gray-700">Name</label>
                <input
                  type="text"
                  value={updateProductData.name}
                  onChange={(e) => setUpdateProductData({ ...updateProductData, name: e.target.value })}
                  className="w-full p-2 border rounded"
                />
              </div>
              <div>
                <label className="block text-gray-700">Price</label>
                <input
                  type="number"
                  value={updateProductData.price}
                  onChange={(e) => setUpdateProductData({ ...updateProductData, price: e.target.value })}
                  className="w-full p-2 border rounded"
                />
              </div>
              <div>
                <label className="block text-gray-700">Description</label>
                <textarea
                  value={updateProductData.description}
                  onChange={(e) => setUpdateProductData({ ...updateProductData, description: e.target.value })}
                  className="w-full p-2 border rounded"
                ></textarea>
              </div>
              <div>
                <label className="block text-gray-700">Stock</label>
                <input
                  type="number"
                  value={updateProductData.stock}
                  onChange={(e) => setUpdateProductData({ ...updateProductData, stock: e.target.value })}
                  className="w-full p-2 border rounded"
                />
              </div>
              <div>
                <label className="block text-gray-700">Category</label>
                <input
                  type="text"
                  value={updateProductData.category}
                  onChange={(e) => setUpdateProductData({ ...updateProductData, category: e.target.value })}
                  className="w-full p-2 border rounded"
                />
              </div>
            </div>
            <div className="mt-6 flex justify-between gap-4">
              <button
                onClick={handleUpdate}
                className="px-4 py-2 bg-green-600 text-white rounded-lg hover:bg-green-700 focus:outline-none focus:ring-2 focus:ring-green-300 transition"
              >
                Save
              </button>
              <button
                onClick={closeUpdateModal}
                className="px-4 py-2 bg-gray-500 text-white rounded-lg hover:bg-gray-600 focus:outline-none focus:ring-2 focus:ring-gray-300 transition"
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

export default ManageProduct;
