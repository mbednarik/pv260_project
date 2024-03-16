import { HoldingDiffsQueryParams } from "~/types/holdingDiff";
import axios from "~/lib/axios";

const fetchHoldingDiffs = async (params: HoldingDiffsQueryParams) => {
	const response = await axios.get("/holding-diffs", {
		params,
	});

	return response.data;
};

export default fetchHoldingDiffs;
