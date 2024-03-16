import { Alert, Button, Center, Flex } from "@mantine/core";
import { IconBug } from "@tabler/icons-react";
import { FallbackProps } from "react-error-boundary";

export const ErrorFallback = ({ error, resetErrorBoundary }: FallbackProps) => (
	<Center h="100%" w="100%">
		<Alert
			color="red"
			title="There has been an error"
			icon={<IconBug />}
			p="xl"
		>
			<Flex gap="sm" direction="column">
				Something went wrong: {error.message}
				<Button
					variant="outline"
					color="red"
					onClick={resetErrorBoundary}
				>
					Try again
				</Button>
			</Flex>
		</Alert>
	</Center>
);
