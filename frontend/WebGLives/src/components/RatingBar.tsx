import { StarIcon } from "@chakra-ui/icons"

export interface RatingBarProps {
    number: Number;
}

export const RatingBar: React.FC<RatingBarProps> = ({number}) => {
    return (
        <div>
            {
                Array(5)
                    .fill("")
                    .map((_, i) => (
                        <StarIcon
                            key={i}
                            color={i < number ? "yellow.500" : "gray.400"}
                        />
                    ))

            }
        </div>
    )
}