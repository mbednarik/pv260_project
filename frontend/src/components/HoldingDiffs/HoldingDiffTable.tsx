import { Box, Table } from "@mantine/core";
import { HoldingDiffTableRow } from "~/components/HoldingDiffs/HoldingDiffTableRow";
import { HoldingDiff } from "~/types/holdingDiff";

type HoldingDiffTableProps = {
	holdingDiffs: HoldingDiff[];
};

export const HoldingDiffTable = ({ holdingDiffs }: HoldingDiffTableProps) => (
	<Box h="100vh" style={{ overflow: "auto" }}>
		<Table>
			<Table.Thead pos="sticky" top={0} bg="blue.1">
				<Table.Tr>
					<Table.Th>Fund name</Table.Th>
					<Table.Th>Company name</Table.Th>
					<Table.Th>Ticker</Table.Th>
					<Table.Th># shares</Table.Th>
					<Table.Th>Shares change %</Table.Th>
					<Table.Th>Weight change</Table.Th>
				</Table.Tr>
			</Table.Thead>
			<Table.Tbody>
				{holdingDiffs.map(holdingDiff => (
					<HoldingDiffTableRow
						key={holdingDiff.id}
						holdingDiff={holdingDiff}
					/>
				))}
			</Table.Tbody>
		</Table>
	</Box>
);
