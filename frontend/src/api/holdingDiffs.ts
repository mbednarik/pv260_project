import { HoldingDiff, HoldingDiffsQueryParams } from "~/types/holdingDiff";
import axios from "~/lib/axios";

const fetchHoldingDiffs = async (params: HoldingDiffsQueryParams) => {
	const response = await axios.get<HoldingDiff[]>("/holdingDiff", {
		params,
	});

	return response.data;
};

export default fetchHoldingDiffs;
