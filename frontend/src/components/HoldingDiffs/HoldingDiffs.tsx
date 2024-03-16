import useHoldingDiffs from "~/hooks/useHoldingDiffs";

export const HoldingDiffs = () => {
	useHoldingDiffs({
		fundId: "fundId",
		oldHoldingsDate: "oldHoldingsDate",
		newHoldingsDate: "newHoldingsDate",
	});

	return <div>HoldingDiffs</div>;
};
