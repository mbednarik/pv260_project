import dayjs from "dayjs";
import axios from "~/lib/axios";
import { HoldingDiff, HoldingDiffsQueryParams } from "~/types/holdingDiff";

const fetchHoldingDiffs = async (params: HoldingDiffsQueryParams) => {
	const oldHoldingDate = dayjs(params.oldHoldingDate).format("YYYY-MM-DD");
	const newHoldingDate = dayjs(params.newHoldingDate).format("YYYY-MM-DD");

	const response = await axios.get<HoldingDiff[]>("/holdingDiff", {
		params: {
			...params,
			oldHoldingDate,
			newHoldingDate,
		},
	});

	return response.data;
};

export default fetchHoldingDiffs;
