import { Alert } from "@mantine/core";
import { HoldingDiffTable } from "~/components/HoldingDiffs/HoldingDiffTable";
import useHoldingDiffs from "~/hooks/useHoldingDiffs";
import { HoldingDiffsQueryParams } from "~/types/holdingDiff";

type HoldingDiffsProps = HoldingDiffsQueryParams;

export const HoldingDiffs = (props: HoldingDiffsProps) => {
	const { data: holdingDiffs } = useHoldingDiffs(props);

	return holdingDiffs.length === 0 ? (
		<Alert>No holding diffs found</Alert>
	) : (
		<HoldingDiffTable holdingDiffs={holdingDiffs} />
	);
};
