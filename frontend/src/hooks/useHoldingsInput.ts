import dayjs from "dayjs";
import uniqBy from "lodash/uniqBy";
import useHoldings from "~/hooks/useHoldings";

export const useHoldingsInput = () => {
	const { data: holdings } = useHoldings();

	const fundIds = uniqBy(holdings, "fundId").map(holding => ({
		label: String(holding.fundId),
		value: String(holding.fundId),
	}));
	const dates = uniqBy(holdings, "date").map(holding => dayjs(holding.date));

	return { fundIds, dates };
};
