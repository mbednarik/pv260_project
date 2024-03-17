import { Box } from "@mantine/core";
import useHoldingDiffs from "~/hooks/useHoldingDiffs";
import { HoldingDiffTable } from "~/components/HoldingDiffs/HoldingDiffTable";

export const HoldingDiffs = () => {
	// TODO: fix this, hardcoded for now
	const { data } = useHoldingDiffs({
		fundId: 1,
		oldHoldingDate: "2024/1/1",
		newHoldingDate: "2024/2/1",
	});

	return (
		<Box p="xl">
			<HoldingDiffTable holdingDiffs={data} />
		</Box>
	);
};
