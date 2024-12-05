/** @type {import('tailwindcss').Config} */
export default {
  content: [
    "./src/**/*.{js,jsx,ts,tsx}", // Tìm kiếm tất cả các file trong thư mục src
  ],
  theme: {
    extend: {}, // Có thể mở rộng theme của Tailwind tại đây nếu cần
  },
  plugins: [], // Có thể thêm các plugin nếu cần
}
