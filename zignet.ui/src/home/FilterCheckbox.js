import React, { Component } from 'react';

class FilterCheckbox extends Component {
	constructor(props) {
		super(props)

		this.handleCheckboxChange = this.handleCheckboxChange.bind(this);

		this.filterName = 'show' + this.props.name;
	}

	handleCheckboxChange(event) {
		this.props.filter[event.target.name] = event.target.checked;
		this.props.onFilterChange(this.props.filter);
	}

  render() {
    return (
			<div className="form-check">
			  <input 
			  	name={this.filterName}
			  	type="checkbox"
			  	checked={this.props.filter[this.filterName]}
			  	onChange={this.handleCheckboxChange}
			  	className="form-check-input"
			  	id={this.filterName}/>
			  <label className="form-check-label" htmlFor={this.filterName}>{this.props.label || this.props.name}</label>
			</div>
    );
  }
}

export default FilterCheckbox;
