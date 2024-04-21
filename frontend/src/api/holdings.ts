import axios from "~/lib/axios";
import { Holding } from "~/types/holding";

const fetchHoldings = async () => {
	const response = await axios.get<{ result: Holding[] }>("/holding");

	return response.data.result;
};

export default fetchHoldings;
