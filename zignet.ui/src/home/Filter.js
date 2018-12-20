import React, { Component } from 'react';

class Filter extends Component {
	constructor(props) {
		super(props)

		this.handleLoopNetChange = this.handleLoopNetChange.bind(this);
		this.handleLmMobileChange = this.handleLmMobileChange.bind(this);
	}

	handleLoopNetChange(event) 	{	
		this.props.filter.showLoopNet = event.target.checked;
		this.props.onFilterChange(this.props.filter);
	}

	handleLmMobileChange(event) {
		this.props.filter.showLmMobile = event.target.checked;
		this.props.onFilterChange(this.props.filter);
	}	

  render() {
    return (
      <div>
				<div className="form-check">
				  <input 
				  	name="showLoopNet"
				  	type="checkbox"
				  	checked={this.props.filter.showLoopNet}
				  	onChange={this.handleLoopNetChange}
				  	className="form-check-input"
				  	id="showLoopNet"/>
				  <label className="form-check-label" htmlFor="showLoopNet">LoopNet</label>
				</div>
				<div className="form-check">
				  <input 
				  	name="showLmMobile"
				  	type="checkbox"
				  	checked={this.props.filter.showLmMobile}
				  	onChange={this.handleLmMobileChange}
				  	className="form-check-input"
				  	id="showLmMobile"/>
				  <label className="form-check-label" htmlFor="showLmMobile">LmMobile</label>
				</div>
      </div>
    );
  }
}

export default Filter;
