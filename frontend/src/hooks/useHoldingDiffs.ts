import { useSuspenseQuery } from "@tanstack/react-query";
import fetchHoldingDiffs from "~/api/holdingDiffs";
import { HoldingDiffsQueryParams } from "~/types/holdingDiff";

const useHoldingDiffs = (params: HoldingDiffsQueryParams) =>
	useSuspenseQuery({
		queryKey: ["holdingDiffs", params],
		queryFn: () => fetchHoldingDiffs(params),
	});

export default useHoldingDiffs;
