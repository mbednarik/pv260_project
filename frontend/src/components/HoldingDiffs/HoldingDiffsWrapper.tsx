import { Alert, Flex, Select, Stack } from "@mantine/core";
import { DatePickerInput } from "@mantine/dates";
import { useState } from "react";
import { HoldingDiffs } from "~/components/HoldingDiffs/HoldingDiffs";
import { useHoldingsInput } from "~/hooks/useHoldingsInput";

export const HoldingDiffsWrapper = () => {
	const { dates, fundIds } = useHoldingsInput();

	const [oldHoldingDate, setOldHoldingDate] = useState<Date | null>(null);
	const [newHoldingDate, setNewHoldingDate] = useState<Date | null>(null);
	const [fundId, setFundId] = useState<string | null>(null);

	const areAllInputsFilled = !!oldHoldingDate && !!newHoldingDate && !!fundId;

	const excludeDates = (date: Date) =>
		!dates.some(d => d.isSame(date, "day"));

	return (
		<Stack m="lg">
			<Flex gap="lg">
				<DatePickerInput
					label="Old holding date"
					placeholder="Pick a date"
					value={oldHoldingDate}
					onChange={setOldHoldingDate}
					excludeDate={excludeDates}
				/>
				<DatePickerInput
					label="New holding date"
					placeholder="Pick a date"
					value={newHoldingDate}
					onChange={setNewHoldingDate}
					excludeDate={excludeDates}
				/>
				<Select
					label="Fund ID"
					placeholder="Enter a number"
					value={fundId}
					data={fundIds}
					onChange={setFundId}
				/>
			</Flex>

			{areAllInputsFilled ? (
				<HoldingDiffs
					oldHoldingDate={oldHoldingDate}
					newHoldingDate={newHoldingDate}
					fundId={fundId}
				/>
			) : (
				<Alert color="red">Please fill in all the inputs</Alert>
			)}
		</Stack>
	);
};
