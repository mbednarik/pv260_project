import baseAxios from "axios";

const axios = baseAxios.create({
	baseURL: import.meta.env.VITE_API_BASE_URL,
});

export default axios;
