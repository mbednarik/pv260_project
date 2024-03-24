import { Box } from "@mantine/core";
import useHoldingDiffs from "~/hooks/useHoldingDiffs";
import { HoldingDiffTable } from "~/components/HoldingDiffs/HoldingDiffTable";

export const HoldingDiffs = () => {
	// TODO: fix this, hardcoded for now
	const { data } = useHoldingDiffs({
		fundId: 2,
		oldHoldingDate: "2024-03-15",
		newHoldingDate: "2024-03-15",
	});

	console.log(data);
	return (
		<Box p="xl">
			<HoldingDiffTable holdingDiffs={data} />
		</Box>
	);
};
